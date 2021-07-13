// import {
//     userService
// } from '@/services/user.service';

const state = () => ({
  isAuthenticated: false,
  userInfo: {
    hasData: false,
    isUserFirstLogin: true,
    socialId: '',
    fullNameAr: '',
    fullNameEn: '',
    UserRole: ''
  },
  returnUrl: '',
  permissions: [],
  isAuthLoading: false
});

const getters = {
  isAuthenticated: state => state.isAuthenticated,
  userRole: state => state.userInfo.userRole,
  userInfo: state => state.userInfo,
  userSocialId: state => state.userInfo.socialId,
  isUserFirstLogin: state => state.userInfo.isUserFirstLogin,
  permissions: state => state.permissions,
  isAuthLoading: state => state.isAuthLoading
};

const mutations = {
  setIsAuthenticated (state, value) {
    state.isAuthenticated = value;
  },
  setIsAuthLoading (state, value) {
    state.isAuthLoading = value;
  },
  updateUserInfo (state, value) {
    state.userInfo = value;
  },
  setReturnUrl (state, value) {
    state.returnUrl = value;
  },
  updateUserPermissions (state, value) {
    state.permissions = value;
  }
};

const actions = {
  async checkUserAuthentication ({ commit }) {
    commit('setIsAuthLoading', true);
    const result = await this.$authenticationService.isAuthenticatedAsync();
    commit('setIsAuthenticated', result.data);
    commit('setIsAuthLoading', false);
  },
  async authenticateUser ({ commit }, returnUrl) {
    commit('setReturnUrl', returnUrl);
    await this.$authenticationService.authenticateAsync(returnUrl);
  },
  login ({ commit }, returnUrl) {
    commit('setReturnUrl', returnUrl);
    this.$authenticationService.login(returnUrl);
  },
  // async getUserProfile({
  //     commit
  // }) {
  //     const result = await userService.getUserProfile();
  //     commit('updateUserInfo', result.data.item1);
  //     commit('updateUserPermissions', result.data.item2);
  // },
  async authorize ({ state }, permission) {
    return state.permissions.includes(permission);
  },
  updatePermissions ({ commit }, permissions) {
    commit('updateUserPermissions', permissions);
  }
};

export default {
  state,
  getters,
  mutations,
  actions
};
