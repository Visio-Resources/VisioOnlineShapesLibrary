from __future__ import annotations
from app.extensions import db
from datetime import datetime
from typing import List
from sqlalchemy import Integer, String, func, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import TYPE_CHECKING
if TYPE_CHECKING:
    from app.models.auth import User


class Stencil(db.Model):
    __tablename__ = "stencils"
    id: Mapped[int] = mapped_column(primary_key=True)
    upload_date: Mapped[datetime] = mapped_column(insert_default=func.now())
    last_update: Mapped[datetime] = mapped_column(nullable=True)
    file_name: Mapped[str] = mapped_column()
    title: Mapped[str] = mapped_column()
    subject: Mapped[str] = mapped_column(String(20))
    author: Mapped[str] = mapped_column()
    manager: Mapped[str] = mapped_column()
    company: Mapped[str] = mapped_column()
    language: Mapped[str] = mapped_column()
    categories: Mapped[str] = mapped_column()
    tags: Mapped[str] = mapped_column(String(512))
    comments: Mapped[str] = mapped_column(String(1024))
    shapes: Mapped[List["Shape"]] = relationship(back_populates="stencil", cascade="all, delete-orphan")
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"), nullable=False)
    user: Mapped["User"] = relationship(back_populates="stencils")

    def __repr__(self) -> str:
        return f"Stencil(id={self.id!r}, name={self.title!r})"


class Shape(db.Model):
    __tablename__ = "shapes"
    id: Mapped[int] = mapped_column(primary_key=True)
    upload_date: Mapped[datetime] = mapped_column(insert_default=func.now())
    last_update: Mapped[datetime] = mapped_column(nullable=True)
    name: Mapped[str] = mapped_column()
    prompt: Mapped[str] = mapped_column()
    keywords: Mapped[str] = mapped_column()
    data_object: Mapped[str] = mapped_column()
    stencil_id: Mapped[int] = mapped_column(ForeignKey("stencils.id"), nullable=True)
    stencil: Mapped["Stencil"] = relationship(back_populates="shapes")
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"), nullable=False)
    user: Mapped["User"] = relationship(back_populates="shapes")

    def __repr__(self) -> str:
        return f"Shape(id={self.id!r}, name={self.name!r})"
    
    def serialize(self):
        return{
            'id': self.id,
            'upload_date': self.upload_date,
            'last_update': self.last_update,
            'name': self.name,
            'prompt': self.prompt,
            'keywords': self.keywords,
            'stencil_id': self.stencil.id if self.stencil else '',
            'stencil_file_name': self.stencil.file_name if self.stencil else '',
            'user': self.user.name
        }


class ShapeDownload(db.Model):
    __tablename__ = "shape_downloads"
    id: Mapped[int] = mapped_column(primary_key=True)
    shape_id: Mapped[int] = mapped_column(ForeignKey("shapes.id"), nullable=False)
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"), nullable=False)
    date: Mapped[datetime] = mapped_column(insert_default=func.now())


class StencilDownload(db.Model):
    __tablename__ = "stencil_downloads"
    id: Mapped[int] = mapped_column(primary_key=True)
    stencil_id: Mapped[int] = mapped_column(ForeignKey("shapes.id"), nullable=False)
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"), nullable=False)
    date: Mapped[datetime] = mapped_column(insert_default=func.now())