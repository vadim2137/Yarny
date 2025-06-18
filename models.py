from enum import Enum
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy.dialects.postgresql import ARRAY

db = SQLAlchemy()

class DifficultyLevel(Enum):
    EASY = 'легкий'
    MEDIUM = 'средний'
    HARD = 'сложный'

class RowStatus(Enum):
    COMPLETED = 'завершён'
    NOT_COMPLETED = 'не завершён'

class User(db.Model):
    __tablename__ = 'users'
    
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    first_name = db.Column(db.String(80))
    last_name = db.Column(db.String(80))
    avatar = db.Column(db.String(255))
    email = db.Column(db.String(120), unique=True, nullable=False)
    password = db.Column(db.String(255), nullable=False)
    subscribers_ids = db.Column(ARRAY(db.Integer))
    subscriptions_ids = db.Column(ARRAY(db.Integer))
    favorite_schemes_id = db.Column(ARRAY(db.Integer))
    
    created_schemes = db.relationship('Scheme', backref='creator', lazy=True)
    active_schemes = db.relationship('ActiveScheme', backref='user', lazy=True)

class Scheme(db.Model):
    __tablename__ = 'schemes'
    
    id = db.Column(db.Integer, primary_key=True)
    creator_id = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=False)
    title = db.Column(db.String(255), nullable=False)
    preview_image = db.Column(db.String(255))
    tags = db.Column(ARRAY(db.String(50)))
    difficulty_level = db.Column(db.Enum(DifficultyLevel))
    materials = db.Column(db.Text)
    symbols_description = db.Column(db.Text)
    
    stages = db.relationship('Stage', backref='scheme', lazy=True, cascade="all, delete-orphan")
    active_instances = db.relationship('ActiveScheme', backref='scheme', lazy=True)

class Stage(db.Model):
    __tablename__ = 'stages'
    
    id = db.Column(db.Integer, primary_key=True)
    scheme_id = db.Column(db.Integer, db.ForeignKey('schemes.id'), nullable=False)
    title = db.Column(db.String(255), nullable=False)
    preview_image = db.Column(db.String(255))
    description = db.Column(db.Text)
    
    rows = db.relationship('Row', backref='stage', lazy=True, cascade="all, delete-orphan")

class Row(db.Model):
    __tablename__ = 'rows'
    
    id = db.Column(db.Integer, primary_key=True)
    stage_id = db.Column(db.Integer, db.ForeignKey('stages.id'), nullable=False)
    title = db.Column(db.String(255), nullable=False)

class ActiveScheme(db.Model):
    __tablename__ = 'schemes_active'
    
    id = db.Column(db.Integer, primary_key=True)
    scheme_id = db.Column(db.Integer, db.ForeignKey('schemes.id'), nullable=False)
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=False)
    
    stages = db.relationship('ActiveStage', backref='active_scheme', lazy=True, cascade="all, delete-orphan")

class ActiveStage(db.Model):
    __tablename__ = 'stages_active'
    
    id = db.Column(db.Integer, primary_key=True)
    active_scheme_id = db.Column(db.Integer, db.ForeignKey('schemes_active.id'), nullable=False)
    stage_id = db.Column(db.Integer, db.ForeignKey('stages.id'), nullable=False)
    
    rows = db.relationship('ActiveRow', backref='active_stage', lazy=True, cascade="all, delete-orphan")

class ActiveRow(db.Model):
    __tablename__ = 'rows_active'
    
    id = db.Column(db.Integer, primary_key=True)
    active_stage_id = db.Column(db.Integer, db.ForeignKey('stages_active.id'), nullable=False)
    row_id = db.Column(db.Integer, db.ForeignKey('rows.id'), nullable=False)
    status = db.Column(db.Enum(RowStatus), default=RowStatus.NOT_COMPLETED)
