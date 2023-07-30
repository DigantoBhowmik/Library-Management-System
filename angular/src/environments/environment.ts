import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Diganto',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44319',
    redirectUri: baseUrl,
    clientId: 'Diganto_App',
    responseType: 'code',
    scope: 'offline_access openid profile role email phone Diganto',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44319',
      rootNamespace: 'Diganto',
    },
  },
} as Environment;
