export default {
  // Target: https://go.nuxtjs.dev/config-target

  loading: true,
  target: 'static',
  publicRuntimeConfig: {
    baseURL:
      process.env.NUXT_BASE_URL ||
      process.env.BASE_URL ||
      'https://sdaia-nuxt-template.herokuapp.com',
  },
  env: {
    baseUrl: 'https://localhost:44340/api/'
  },
  router: {
    middleware: ['guard'],
  },

  // Global page headers: https://go.nuxtjs.dev/config-head
  head: {
    titleTemplate: '%s - knowledge-hub',
    title: 'knowledge-hub',
    htmlAttrs: {
      lang: 'en',
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
    ],
    link: [{ rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }],
  },

  // Global CSS: https://go.nuxtjs.dev/config-css
  css: [
    '@/assets/global.scss'
  ],

  // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
  plugins: [
    '~/plugins/i18n.js',
    '~/plugins/axios.js',
    '~/plugins/axiosServices.js',
    '~/plugins/helpers.js',
    '~/plugins/vue-breadcrumbs.js',
    '~/plugins/logger.js',
    '~/plugins/auth.js',
    { src: '~/plugins/apexCharts.js', mode: 'client' },
    { src: '~/plugins/gsap.js', mode: 'client' },
  ],

  // Auto import components: https://go.nuxtjs.dev/config-components
  components: true,

  // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
  buildModules: [
    // https://go.nuxtjs.dev/vuetify
    '@nuxtjs/vuetify',
  ],

  // Modules: https://go.nuxtjs.dev/config-modules
  modules: [
    // https://go.nuxtjs.dev/axios
    '@nuxtjs/axios',
    'nuxt-i18n',
    [
      'vue-toastification/nuxt',
      {
        position: 'top-center',
        timeout: 2500,
        closeOnClick: true,
        pauseOnFocusLoss: true,
        pauseOnHover: true,
        draggable: true,
        draggablePercent: 0.6,
        showCloseButtonOnHover: false,
        hideProgressBar: false,
        closeButton: 'button',
        icon: true,
        // rtl: true,
      },
    ],
  ],

  i18n: {
    locales: ['en', 'ar'],
    defaultLocale: 'ar',
    vueI18n: {
      fallbackLocale: 'ar',
      messages: {
        en: {},
        ar: {},
      },
    },
  },

  // Axios module configuration: https://go.nuxtjs.dev/config-axios
  axios: {},

  // Vuetify module configuration: https://go.nuxtjs.dev/config-vuetify
  vuetify: {
    customVariables: ['~/assets/variables.scss'],
    theme: {
      themes: {
        light: {
          primary: '#273470',
          secondary: '#6258a6',
          background: '#f8f8f8',
          navBackground: '#f2f2f4',
          lightGrey: '#e5e6ec',
          textGrey: '#7b7b7b',
          error: '#d4475a',
          lightGreen: '#ccf6e4',
          lightOrange: '#ffc1ab',
          orange: '#ed6f43',
          lightSecondary: '#8680c4',
          darkBlue: '#4153a8',
          lightError: '#ffc6cd',
          lightBlue: '#42b5ff',
          superLightBlue: '#d4eeff',
        },
      },
    },
  },

  // Build Configuration: https://go.nuxtjs.dev/config-build
  build: {},
};
