<template>
  <v-row>
    <Breadcrumbs />
    <v-col cols="12"> <NuxtChild /></v-col>
  </v-row>
</template>

<script>
import { mapActions } from 'vuex';
export default {
  components: {
    Breadcrumbs: () => import('../components/common/Breadcrumbs.vue'),
  },
  watch: {
    $route: {
      deep: true,
      immediate: true,
      handler(val) {
        const path = '/example';
        if (val.path === path) {
          this.setCrumbs([
            {
              title: 'Example Page',
              path,
            },
          ]);
        }
      },
    },
  },
  data() {
    return {
      crumbs: null,
    };
  },
  methods: {
    ...mapActions(['setCrumbs']),
  },
  computed: {},
  mounted() {
    this.setCrumbs([
      {
        title: 'Example Page',
        path: '/example',
      },
    ]);
  },
};
</script>