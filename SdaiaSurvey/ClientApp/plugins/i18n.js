import Vue from 'vue'
import VueI18n from 'vue-i18n'
import ar from './locales/ar.json'
import en from './locales/en.json'

Vue.use(VueI18n)

const messages = {
  ar,
  en,
}

const i18n = new VueI18n({
  locale: 'en', // set locale
  messages, // set locale messages
})

export default i18n
