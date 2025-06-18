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

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=False)
