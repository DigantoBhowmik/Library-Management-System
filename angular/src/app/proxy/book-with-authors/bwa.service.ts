import type { BookWithAuthorDto, CreateUpdateBWADto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BWAService {
  apiName = 'Default';

  create = (input: CreateUpdateBWADto) =>
    this.restService.request<any, BookWithAuthorDto>({
      method: 'POST',
      url: '/api/app/b-wA',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/b-wA/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, BookWithAuthorDto>({
      method: 'GET',
      url: `/api/app/b-wA/${id}`,
    },
    { apiName: this.apiName });

  getCustomAsynById = (id: string) =>
    this.restService.request<any, BookWithAuthorDto[]>({
      method: 'GET',
      url: `/api/app/b-wA/${id}/custom-asyn`,
    },
    { apiName: this.apiName });

  getList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<BookWithAuthorDto>>({
      method: 'GET',
      url: '/api/app/b-wA',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateBWADto) =>
    this.restService.request<any, BookWithAuthorDto>({
      method: 'PUT',
      url: `/api/app/b-wA/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
