
const state = () => ({
})

export const getters = {
}

const mutations = {
}

const actions = {
    async getPosts() {
        const response = await this.$aboutService.getPosts();
        return response;
    },
    async getPost(_,id) {
        const response = await this.$aboutService.getPost(id);
        return response;
    },
}

export default {
    state,
    getters,
    mutations,
    actions
}