import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdatePublisherDto {
  name?: string;
}

export interface PublisherDto extends AuditedEntityDto<string> {
  name?: string;
}
