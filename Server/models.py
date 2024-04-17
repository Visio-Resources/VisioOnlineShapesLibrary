from datetime import datetime
from typing import List
from sqlalchemy import func
from sqlalchemy import ForeignKey
from sqlalchemy import String
from sqlalchemy import Integer
from sqlalchemy.orm import DeclarativeBase
from sqlalchemy.orm import Mapped
from sqlalchemy.orm import mapped_column
from sqlalchemy.orm import relationship

class Base(DeclarativeBase):
    pass

class Stencil(Base):
    __tablename__ = "stencils"
    id: Mapped[int] = mapped_column(primary_key=True)
    uploadDate: Mapped[datetime] = mapped_column(insert_default=func.now())
    downloadCounter: Mapped[int] = mapped_column(Integer, insert_default=0)
    fileName: Mapped[str] = mapped_column(String(32))
    title: Mapped[str] = mapped_column(String(32))
    subject: Mapped[str] = mapped_column(String(20))
    author: Mapped[str] = mapped_column(String(32))
    manager: Mapped[str] = mapped_column(String(32))
    company: Mapped[str] = mapped_column(String(32))
    language: Mapped[str] = mapped_column(String(32))
    categories: Mapped[str] = mapped_column(String(512))
    tags: Mapped[str] = mapped_column(String(512))
    comments: Mapped[str] = mapped_column(String(1024))
    masters: Mapped[List["Master"]] = relationship(back_populates="stencil", cascade="all, delete-orphan")
    def __repr__(self) -> str:
        return f"Stencil(id={self.id!r}, name={self.title!r})"

class Master(Base):
    __tablename__ = "masters"
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(String(32))
    prompt: Mapped[str] = mapped_column(String(512))
    keywords: Mapped[str] = mapped_column(String(512))
    dataObject: Mapped[str] = mapped_column(String(60000))
    stencilId: Mapped[int] = mapped_column(ForeignKey("stencils.id"))
    stencil: Mapped["Stencil"] = relationship(back_populates="masters")
    def __repr__(self) -> str:
        return f"Master(id={self.id!r}, name={self.name!r})"