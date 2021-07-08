export default class AboutService {
  constructor($axios) {
    this.$axios = $axios;
  }

  getPosts() {
    return this.$axios.$get('https://api.nuxtjs.dev/posts');
  }
  getPost(id) {
    return this.$axios.$get('https://api.nuxtjs.dev/posts/' + id);
  }
}
