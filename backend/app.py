from flask import Flask, request, jsonify
from flask_migrate import Migrate
from models import db, User, Scheme, Stage, Row, ActiveScheme, ActiveStage, ActiveRow
from schemas import (
    UserCreate, User, SchemeCreate, Scheme, StageCreate, Stage, 
    ActiveSchemeCreate, ActiveScheme, ActiveStageCreate, ActiveStage,
    ActiveRowBase, ActiveRow, RowStatus
)
from config import Config
from pydantic import ValidationError
from werkzeug.security import generate_password_hash

app = Flask(__name__)
app.config.from_object(Config)
db.init_app(app)
migrate = Migrate(app, db)

@app.route('/users', methods=['POST'])
def create_user():
    try:
        user_data = UserCreate(**request.json)
    except ValidationError as e:
        return jsonify({'error': str(e)}), 400
    
    hashed_password = generate_password_hash(user_data.password)
    user = User(
        username=user_data.username,
        email=user_data.email,
        password=hashed_password,
        first_name=user_data.first_name,
        last_name=user_data.last_name
    )
    
    db.session.add(user)
    db.session.commit()
    
    return jsonify(User.from_orm(user).dict()), 201

@app.route('/users/<int:user_id>/schemes', methods=['GET'])
def get_user_schemes(user_id):
    page = request.args.get('page', 1, type=int)
    per_page = request.args.get('per_page', 10, type=int)
    
    # Проверяем существование пользователя
    user = User.query.get(user_id)
    if not user:
        return jsonify({"error": "User not found"}), 404
    
    # Получаем схемы с пагинацией
    schemes_query = Scheme.query.filter_by(creator_id=user_id)
    paginated_schemes = schemes_query.paginate(page=page, per_page=per_page, error_out=False)
    
    # Форматируем ответ
    response = {
        "items": [format_scheme(scheme) for scheme in paginated_schemes.items],
        "total": paginated_schemes.total,
        "page": paginated_schemes.page,
        "per_page": paginated_schemes.per_page,
        "creator": {
            "id": user.id,
            "username": user.username,
            "avatar": user.avatar
        }
    }
    
    return jsonify(response)

def format_scheme(scheme):
    return {
        "id": scheme.id,
        "title": scheme.title,
        "preview_image": scheme.preview_image,
        "difficulty_level": scheme.difficulty_level.value,
        "tags": scheme.tags,
        "created_at": scheme.created_at.isoformat() if scheme.created_at else None,
        "materials": scheme.materials,
        "stages_count": len(scheme.stages)
    }

@app.route('/users/<int:user_id>', methods=['GET'])
def get_user(user_id):
    user = User.query.get_or_404(user_id)
    return jsonify(User.from_orm(user).dict())

@app.route('/schemes', methods=['POST'])
def create_scheme():
    try:
        scheme_data = SchemeCreate(**request.json)
    except ValidationError as e:
        return jsonify({'error': str(e)}), 400
    
    scheme = Scheme(
        creator_id=scheme_data.creator_id,
        title=scheme_data.title,
        preview_image=scheme_data.preview_image,
        tags=scheme_data.tags,
        difficulty_level=scheme_data.difficulty_level,
        materials=scheme_data.materials,
        symbols_description=scheme_data.symbols_description
    )
    
    for stage_data in scheme_data.stages:
        stage = Stage(
            title=stage_data.title,
            preview_image=stage_data.preview_image,
            description=stage_data.description
        )
        
        for row_data in stage_data.rows:
            row = Row(title=row_data.title)
            stage.rows.append(row)
        
        scheme.stages.append(stage)
    
    db.session.add(scheme)
    db.session.commit()
    
    return jsonify(Scheme.from_orm(scheme).dict()), 201

@app.route('/schemes/<int:scheme_id>', methods=['GET'])
def get_scheme(scheme_id):
    scheme = Scheme.query.get_or_404(scheme_id)
    return jsonify(Scheme.from_orm(scheme).dict())

@app.route('/users/<int:user_id>/active_schemes', methods=['POST'])
def create_active_scheme(user_id):
    try:
        active_scheme_data = ActiveSchemeCreate(**request.json)
    except ValidationError as e:
        return jsonify({'error': str(e)}), 400
    
    # Проверяем, что схема существует
    scheme = Scheme.query.get_or_404(active_scheme_data.scheme_id)
    
    # Создаем активную схему
    active_scheme = ActiveScheme(
        scheme_id=active_scheme_data.scheme_id,
        user_id=user_id
    )
    
    # Копируем структуру оригинальной схемы
    for stage in scheme.stages:
        active_stage = ActiveStage(stage_id=stage.id)
        
        for row in stage.rows:
            active_row = ActiveRow(
                row_id=row.id,
                status=RowStatus.not_completed
            )
            active_stage.rows.append(active_row)
        
        active_scheme.stages.append(active_stage)
    
    db.session.add(active_scheme)
    db.session.commit()
    
    return jsonify(ActiveScheme.from_orm(active_scheme).dict()), 201

@app.route('/active_schemes/<int:active_scheme_id>', methods=['GET'])
def get_active_scheme(active_scheme_id):
    active_scheme = ActiveScheme.query.get_or_404(active_scheme_id)
    return jsonify(ActiveScheme.from_orm(active_scheme).dict())

@app.route('/active_rows/<int:active_row_id>', methods=['PATCH'])
def update_active_row(active_row_id):
    try:
        row_data = ActiveRowBase(**request.json)
    except ValidationError as e:
        return jsonify({'error': str(e)}), 400
    
    active_row = ActiveRow.query.get_or_404(active_row_id)
    active_row.status = row_data.status
    
    db.session.commit()
    
    return jsonify(ActiveRow.from_orm(active_row).dict())

@app.route('/users/<int:user_id>/active_schemes', methods=['GET'])
def get_user_active_schemes(user_id):
    active_schemes = ActiveScheme.query.filter_by(user_id=user_id).all()
    return jsonify([ActiveScheme.from_orm(scheme).dict() for scheme in active_schemes])

@app.route('/schemes/recommended', methods=['GET'])
def get_recommended_schemes():
    page = request.args.get('page', 1, type=int)
    per_page = request.args.get('per_page', 10, type=int)
    
    # Для авторизованных пользователей
    if 'Authorization' in request.headers:
        try:
            token = request.headers['Authorization'].split()[1]
            user_id = get_jwt_identity()
            user = User.query.get(user_id)
            
            # 1. Схемы с тегами из избранного
            favorite_tags = set()
            for scheme_id in user.favorite_schemes_id or []:
                scheme = Scheme.query.get(scheme_id)
                if scheme and scheme.tags:
                    favorite_tags.update(scheme.tags)
            
            if favorite_tags:
                query = Scheme.query.filter(Scheme.tags.overlap(list(favorite_tags)))
                recommended = query.order_by(db.func.random()).paginate(page=page, per_page=per_page, error_out=False)
                if recommended.items:
                    return jsonify(format_recommended_response(recommended))
            
            # 2. Схемы с предпочитаемым уровнем сложности
            difficulty = get_user_preferred_difficulty(user_id)
            if difficulty:
                query = Scheme.query.filter_by(difficulty_level=difficulty)
                recommended = query.order_by(db.func.random()).paginate(page=page, per_page=per_page, error_out=False)
                if recommended.items:
                    return jsonify(format_recommended_response(recommended))
        
        except Exception as e:
            app.logger.error(f"Error in recommendations for auth user: {str(e)}")
    
    # 3. Для всех (популярные схемы) и для неавторизованных
    return get_popular_schemes(page, per_page)

def get_user_preferred_difficulty(user_id):
    # Анализируем активные схемы пользователя для определения предпочитаемого уровня
    result = db.session.query(
        Scheme.difficulty_level,
        db.func.count().label('count')
    ).join(ActiveScheme).filter(
        ActiveScheme.user_id == user_id
    ).group_by(
        Scheme.difficulty_level
    ).order_by(
        db.desc('count')
    ).first()
    
    return result[0] if result else None

def get_popular_schemes(page, per_page):
    # Схемы, отсортированные по количеству активных пользователей
    popular_query = db.session.query(
        Scheme,
        db.func.count(ActiveScheme.id).label('active_count')
    ).outerjoin(
        ActiveScheme
    ).group_by(
        Scheme.id
    ).order_by(
        db.desc('active_count'),
        db.func.random()
    )
    
    # Для неавторизованных - баланс по сложности
    if 'Authorization' not in request.headers:
        # Получаем по 3 схемы каждого уровня сложности
        easy = Scheme.query.filter_by(difficulty_level=DifficultyLevel.EASY).order_by(db.func.random()).limit(3)
        medium = Scheme.query.filter_by(difficulty_level=DifficultyLevel.MEDIUM).order_by(db.func.random()).limit(3)
        hard = Scheme.query.filter_by(difficulty_level=DifficultyLevel.HARD).order_by(db.func.random()).limit(4)
        
        schemes = easy.union(medium).union(hard).all()
        total = Scheme.query.count()
        
        return jsonify({
            "items": [format_scheme(scheme) for scheme in schemes],
            "total": total,
            "page": 1,
            "per_page": 10
        })
    
    # Для авторизованных (если предыдущие рекомендации не сработали)
    paginated = popular_query.paginate(page=page, per_page=per_page, error_out=False)
    return jsonify(format_recommended_response(paginated))

def format_recommended_response(paginated_result):
    return {
        "items": [format_scheme(item[0]) for item in paginated_result.items],
        "total": paginated_result.total,
        "page": paginated_result.page,
        "per_page": paginated_result.per_page
    }

def format_scheme(scheme):
    return {
        "id": scheme.id,
        "title": scheme.title,
        "preview_image": scheme.preview_image,
        "difficulty_level": scheme.difficulty_level.value,
        "tags": scheme.tags,
        "creator": {
            "id": scheme.creator.id,
            "username": scheme.creator.username
        }
    }

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=False)
