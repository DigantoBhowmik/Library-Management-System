import type { AuditedEntityDto } from '@abp/ng.core';

export interface BookWithAuthorDto extends AuditedEntityDto<string> {
  authorId?: string;
  authorName?: string;
  publisherId?: string;
}

export interface CreateUpdateBWADto {
  authorId?: string;
  publisherId?: string;
}
