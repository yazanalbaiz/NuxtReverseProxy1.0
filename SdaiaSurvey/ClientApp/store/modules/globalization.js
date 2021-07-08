import i18n from '@/plugins/i18n';
// import vuetify from '@/plugins/vuetify';
// import {
//     userService
// } from '@/services/user.service';


const state = () => ({
    language: 'ar',
    rtl: true,
})

export const getters = {
    language: state => state.language,
    rtl: state => state.rtl,
    alignment: state => state.rtl ? 'right' : 'left',
    getReactiveString: () => (key,extra) => {
        return i18n.t(key,extra);
    },
    otherLanguageString: state => state.language === "ar" ? "English" : "العربية",
}

const mutations = {
    setLanguage(state, value) {
        state.language = value;
    },
    setRtl(state, value) {
        state.rtl = value;
    },
}

const actions = {
    async changeLanguage({
        commit
    }, value) {
        if (!value || !languages.includes(value)) {
            return;
        }
        let newRtl = true;
        if (value === 'en') {
            newRtl = false;
        }
        // await userService.setLanguage(value);
        commit('setLanguage', value);
        commit('setRtl', newRtl)
        i18n.locale = value;
        // vuetify.framework.rtl = newRtl;
    },
    changeRtl({
        commit
    }, value) {
        if (value === null || typeof value === 'undefined') {
            return;
        }
        commit('setRtl', value);
    },
    async getLanguage({
        commit,
        getters,
    }) {
        // const response = await userService.getLanguage();
        // let language = response.data;
        let language = null;
        if (!language || !languages.includes(language)) {
            language = 'ar';
        }
        commit('setLanguage', language);
        i18n.locale = language;

        let newRtl = true;
        if (language === 'en') {
            newRtl = false;
        }

        commit('setRtl', newRtl)
        // vuetify.framework.rtl = newRtl;


        return getters.language;
    }
}

const languages = [
    'ar', 'en'
]

export default {
    state,
    getters,
    mutations,
    actions
}