<template>
  <v-row>
    <v-col cols="12" v-if="loading">
      <v-progress-circular></v-progress-circular>
    </v-col>
    <v-col class="text-center">
      {{ getReactiveString('home') }}
      <br />
      <NuxtLink v-for="(post, i) in posts" :key="i" :to="`/example/${post.id}`">
        {{ post.title }} <br />
      </NuxtLink>
    </v-col>
    <v-col cols="12">
      <Barchart/>
    </v-col>
  </v-row>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import MetaImage from '../../assets/exampleImage.jpg';
import Barchart from '@/components/charts/Barchart.vue';

export default {
  name: 'index',
  components: {Barchart},
  data() {
    return {
      loading: false,
      title: 'This is a test page, by Yazan.',
      posts: null,
    };
  },
  computed: {
    ...mapGetters({
      getReactiveString: 'getReactiveString',
      name: 'names/name',
    }),
    ids() {
      return new Array(Math.floor(Math.random() * 10) + 1);
    },
  },
  head() {
    return {
      title: this.title,
      meta: [
        {
          hid: 'description',
          name: 'description',
          content: 'This is a test description, by Yazan.',
        },
        {
          hid: 'robots',
          name: 'robots',
          content: 'index, follow',
        },
        {
          hid: 'og:title',
          property: 'og:title',
          content: 'This is a test og:title, by Yazan.',
        },
        {
          hid: 'og:type',
          property: 'og:type',
          content: 'This is a test og:type, by Yazan.',
        },
        {
          hid: 'og:description',
          property: 'og:description',
          content: 'This is a test og:description, by Yazan.',
        },
        {
          hid: 'og:image',
          property: 'og:image',
          itemprop: 'image',
          content: this.$config.baseURL + MetaImage,
        },
        {
          hid: 'og:url',
          property: 'og:url',
          content: 'This is a test og:url, by Yazan.',
        },
        {
          hid: 'og:site_name',
          property: 'og:site_name',
          content: 'This is a test og:site_name, by Yazan.',
        },
        {
          hid: 'twitter:title',
          name: 'twitter:title',
          content: 'This is a test twitter title, by Yazan.',
        },
        {
          hid: 'twitter:description',
          name: 'twitter:description',
          content: 'This is a test twitter description, by Yazan.',
        },
        {
          hid: 'twitter:image',
          name: 'twitter:image',
          itemprop: 'image',
          content: this.$config.baseURL + MetaImage,
        },
        {
          hid: 'twitter:site',
          name: 'twitter:site',
          content: 'This is a test twitter site, by Yazan.',
        },
        {
          hid: 'twitter:creator',
          name: 'twitter:creator',
          content: 'This is a test twitter creator, by Yazan.',
        },
      ],
    };
  },
  methods: {
    ...mapActions({ getPosts: 'getPosts', getName: 'names/get' }),
    start() {
      this.loading = true;
    },
    finish() {
      this.loading = false;
    },
  },
  mounted() {
    this.getPosts().then((res) => {
      this.posts = res;
    });
    this.getName();
    this.$log('this is an example: ',this.$route,this.$router);
  },
};
</script>
