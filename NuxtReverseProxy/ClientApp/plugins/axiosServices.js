import AboutService from '@/services/aboutService'
export default function ({ $axios }, inject) {
  const aboutService = new AboutService($axios);

  // Will be available in the components as this.$myService
  inject('aboutService', aboutService)
}
