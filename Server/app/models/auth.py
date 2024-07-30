from __future__ import annotations
from app.extensions import db
from datetime import datetime
from typing import List
from sqlalchemy import Integer, String, func, ForeignKey, Column, Table
from sqlalchemy.orm import Mapped, mapped_column, relationship
from flask_login import UserMixin
from app.models.visio import Shape, Stencil

user_team_table = Table(
    "user_team_table",
    db.Model.metadata,
    Column("user_id", ForeignKey("users.id")),
    Column("team_id", ForeignKey("teams.id"))
)

user_role_table = Table(
    "user_role_table",
    db.Model.metadata,
    Column("user_id", ForeignKey("users.id")),
    Column("role_id", ForeignKey("roles.id"))
)

class User(db.Model, UserMixin):
    __tablename__ = "users"
    id: Mapped[int] = mapped_column(primary_key=True)
    register_date: Mapped[datetime] = mapped_column(insert_default=func.now())
    last_active: Mapped[datetime] = mapped_column(nullable=True)
    name: Mapped[str] = mapped_column(unique=True, nullable=False)
    email: Mapped[str] = mapped_column(unique=True, nullable=False)
    password_hash: Mapped[str] = mapped_column(nullable=False)
    token: Mapped[str] = mapped_column(nullable=True)
    message: Mapped[str] = mapped_column(String(512), nullable=True)
    link: Mapped[str] = mapped_column(String(512), nullable=True)
    teams: Mapped[List[Team]] = relationship(secondary=user_team_table, back_populates="users")
    roles: Mapped[List[Role]] = relationship(secondary=user_role_table, back_populates="users")
    shapes: Mapped[List["Shape"]] = relationship(back_populates="user", cascade="all, delete-orphan")
    stencils: Mapped[List["Stencil"]] = relationship(back_populates="user", cascade="all, delete-orphan")
    def __repr__(self) -> str:
        return f"User(id={self.id!r}, name={self.name!r})"
    def get_id(self):
        return self.id

class Team(db.Model):
    __tablename__ = "teams"
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(unique=True, nullable=False)
    description: Mapped[str] = mapped_column(nullable=False)
    users: Mapped[List[User]] = relationship(secondary=user_team_table, back_populates="teams")

class Role(db.Model):
    __tablename__ = "roles"
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(unique=True, nullable=False)
    description: Mapped[str] = mapped_column(nullable=False)
    users: Mapped[List[User]] = relationship(secondary=user_role_table, back_populates="roles")