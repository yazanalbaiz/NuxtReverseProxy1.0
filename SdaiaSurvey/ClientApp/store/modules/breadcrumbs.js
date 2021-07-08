const state = () => ({
  crumbs: null,
});

export const getters = {
  crumbs: (state) => state.crumbs,
};

const mutations = {
  setCrumbs(state, payload) {
    state.crumbs = payload;
  },
};

const actions = {
  setCrumbs({ commit }, payload) {
    commit('setCrumbs', payload);
  },
};

export default {
  state,
  getters,
  mutations,
  actions,
};
