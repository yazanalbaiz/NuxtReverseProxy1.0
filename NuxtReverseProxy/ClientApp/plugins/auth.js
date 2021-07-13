import { AuthenticationService } from '@/services/authenticationService';
export default function ({ app, $axios }, inject) {
  $axios.setBaseURL(process.env.baseUrl);
  const authenticationService = new AuthenticationService(app.router, $axios);

  // Will be available in the components as this.$authenticationService
  inject('authenticationService', authenticationService);
}
