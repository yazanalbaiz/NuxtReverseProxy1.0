<template>
  <v-row>
    <v-col cols="12" v-if="loading">
      <v-progress-circular></v-progress-circular>
    </v-col>
    <v-col class="text-center" v-if="title">
      {{ title }}
    </v-col>
    <v-col cols="12" class="text-center" v-if="post">
      <img :src="post.image" v-if="post.image" />
    </v-col>
  </v-row>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'Dynamic',
  components: {},
  async asyncData({ params, store }) {
    try {
      const post = await store.dispatch('getPost', params.id);
      store.dispatch('setCrumbs', [
        {
          title: 'Example Page',
          path: '/example',
        },
        {
          title: post.title,
          path: '',
        },
      ]);
      return {
        post,
      };
    } catch (err) {
      console.log(err);
    }
  },
  data() {
    return {
      loading: false,
      post: null,
    };
  },
  computed: {
    ...mapGetters(['getReactiveString']),
    id() {
      return this.$route.params.id;
    },
    title() {
      return this.post ? this.post.title : '';
    },
    description() {
      return this.post ? this.post.description : '';
    },
  },
  head() {
    return {
      title: this.title,
      meta: [
        {
          hid: 'description',
          name: 'description',
          content: this.description,
        },
        {
          hid: 'robots',
          name: 'robots',
          content: 'index, follow',
        },
        {
          hid: 'twitter:title',
          name: 'twitter:title',
          content: this.title,
        },
        {
          hid: 'og:title',
          property: 'og:title',
          content: this.title,
        },
      ],
    };
  },
  methods: {
    ...mapActions(['setCrumbs']),
    start() {
      this.loading = true;
    },
    finish() {
      this.loading = false;
    },
  },
  mounted() {
    if(this.post) {
      this.setCrumbs( [
        {
          title: 'Example Page',
          path: '/example',
        },
        {
          title: this.title,
          path: '',
        },
      ]);
    }
  },
};
</script>
