from pydantic import BaseModel, EmailStr
from typing import List, Optional
from enum import Enum

class DifficultyLevel(str, Enum):
    easy = "легкий"
    medium = "средний"
    hard = "сложный"

class RowStatus(str, Enum):
    completed = "завершён"
    not_completed = "не завершён"

class UserBase(BaseModel):
    username: str
    email: EmailStr
    first_name: Optional[str] = None
    last_name: Optional[str] = None

class UserCreate(UserBase):
    password: str

class User(UserBase):
    id: int
    avatar: Optional[str] = None
    subscribers_ids: List[int] = []
    subscriptions_ids: List[int] = []
    favorite_schemes_id: List[int] = []

    class Config:
        from_attributes = True

class RowBase(BaseModel):
    title: str

class Row(RowBase):
    id: int

    class Config:
        from_attributes = True

class StageBase(BaseModel):
    title: str
    description: Optional[str] = None
    preview_image: Optional[str] = None

class StageCreate(StageBase):
    rows: List[RowBase] = []

class Stage(StageBase):
    id: int
    rows: List[Row] = []

    class Config:
        from_attributes = True

class SchemeBase(BaseModel):
    title: str
    tags: List[str] = []
    difficulty_level: DifficultyLevel
    materials: str
    symbols_description: str

class SchemeCreate(SchemeBase):
    preview_image: Optional[str] = None
    stages: List[StageCreate] = []

class Scheme(SchemeBase):
    id: int
    creator_id: int
    preview_image: Optional[str] = None
    stages: List[Stage] = []

    class Config:
        from_attributes = True

class ActiveRowBase(BaseModel):
    row_id: int
    status: RowStatus = RowStatus.not_completed

class ActiveRow(ActiveRowBase):
    id: int

    class Config:
        from_attributes = True

class ActiveStageBase(BaseModel):
    stage_id: int

class ActiveStageCreate(ActiveStageBase):
    rows: List[ActiveRowBase] = []

class ActiveStage(ActiveStageBase):
    id: int
    rows: List[ActiveRow] = []

    class Config:
        from_attributes = True

class ActiveSchemeBase(BaseModel):
    scheme_id: int

class ActiveSchemeCreate(ActiveSchemeBase):
    stages: List[ActiveStageCreate] = []

class ActiveScheme(ActiveSchemeBase):
    id: int
    user_id: int
    stages: List[ActiveStage] = []

    class Config:
        from_attributes = True
