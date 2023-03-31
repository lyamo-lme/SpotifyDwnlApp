import { createUserManager } from 'redux-oidc';

const userManagerConfig = {
  client_id: 'spa.client',
  redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/callback`,
  response_type: 'id_token token',
  scope: 'openid profile',
  authority: 'https://localhost:7187',
  silent_redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/silent_renew.html`,
  automaticSilentRenew: false,
  filterProtocolClaims: false,  
  loadUserInfo: true,
};

const userManager = createUserManager(userManagerConfig);

export default userManager;