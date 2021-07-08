
const state = () => ({
    name: null
})

export const getters = {
    name: state => state.name
}

const mutations = {
    setName(state, payload) {
        state.name = payload
    }
}

const actions = {
    get({commit}) {
        const names = ['peter','reid','mark','tony','mike','john','sam','seth','blake','chad','chase'];
        commit('setName',names.sort( () => .5 - Math.random() )[0]);
    },
}

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
}