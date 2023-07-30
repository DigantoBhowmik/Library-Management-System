import type { AuditedEntityDto } from '@abp/ng.core';
import type { BookType } from './book-type.enum';
import type { AuthorDto } from '../authors/models';

export interface BookDto extends AuditedEntityDto<string> {
  publisherId?: string;
  publisherName?: string;
  name?: string;
  type: BookType;
  publishDate?: string;
  price: number;
  authors: AuthorDto[];
}

export interface CreateUpdateBookDto {
  publisherId?: string;
  name: string;
  type: BookType;
  publishDate: string;
  price: number;
  authors: string[];
}
