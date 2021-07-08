<template>
  <v-app dark>
    <div v-if="!isAuthLoading && isAuthenticated">
      <h1 v-if="error.statusCode === 404">
        {{ pageNotFound }}
      </h1>
      <h1 v-else>
        {{ otherError }}
      </h1>
    </div>
    <v-main v-else>
      <v-container>
        <div class="d-flex justify-center align-center pa-5">
          <v-progress-circular
            :size="50"
            color="primary"
            indeterminate
          ></v-progress-circular>
        </div>
      </v-container>
    </v-main>
    <NuxtLink to="/"> Home page </NuxtLink>
  </v-app>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  layout: 'empty',
  props: {
    error: {
      type: Object,
      default: null,
    },
  },
  data() {
    return {
      pageNotFound: '404 Not Found',
      otherError: 'An error occurred',
    };
  },
  computed: {
    ...mapGetters(['isAuthLoading', 'isAuthenticated']),
  },
  head() {
    const title =
      this.error.statusCode === 404 ? this.pageNotFound : this.otherError;
    return {
      title,
    };
  },
};
</script>

<style scoped>
h1 {
  font-size: 20px;
}
</style>
