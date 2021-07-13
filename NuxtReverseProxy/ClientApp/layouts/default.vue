<template>
  <v-app dark>
    <v-navigation-drawer
      v-model="drawer"
      :mini-variant="miniVariant"
      :clipped="clipped"
      fixed
      app
    >
      <v-list>
        <v-list-item
          v-for="(item, i) in items"
          :key="i"
          :to="item.to"
          router
          exact
        >
          <v-list-item-action>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title v-text="item.title" />
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-app-bar :clipped-left="clipped" fixed app>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-btn icon @click.stop="miniVariant = !miniVariant">
        <v-icon>mdi-{{ `chevron-${miniVariant ? 'right' : 'left'}` }}</v-icon>
      </v-btn>
      <v-btn icon @click.stop="clipped = !clipped">
        <v-icon>mdi-application</v-icon>
      </v-btn>
      <v-btn icon @click.stop="fixed = !fixed">
        <v-icon>mdi-minus</v-icon>
      </v-btn>
      <v-toolbar-title v-text="title" />
      <v-spacer />
      <v-btn icon @click.stop="rightDrawer = !rightDrawer">
        <v-icon>mdi-menu</v-icon>
      </v-btn>
      <v-btn v-if="isAuthenticated" color="primary" href="/api/account/logout">
        تسجيل الخروج
      </v-btn>
    </v-app-bar>
    <v-main v-if="!isAuthLoading">
      <v-container>
        <nuxt />
      </v-container>
    </v-main>
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
    <v-navigation-drawer v-model="rightDrawer" :right="right" temporary fixed>
      <v-list>
        <v-list-item @click.native="right = !right">
          <v-list-item-action>
            <v-icon light> mdi-repeat </v-icon>
          </v-list-item-action>
          <v-list-item-title>Switch drawer (click me)</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-footer :absolute="!fixed" app>
      <span>&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>
  </v-app>
</template>

<script>
import '@mdi/font/css/materialdesignicons.css';
import { mapActions, mapGetters } from 'vuex';
export default {
  async created() {
    await this.checkUserAuthentication();
    if (!this.isAuthenticated) {
      this.login('/');
    }
  },
  methods: {
    ...mapActions(['checkUserAuthentication', 'login']),
  },
  computed: {
    ...mapGetters(['isAuthenticated', 'isAuthLoading']),
    crumbs() {
      const fullPath = this.$route.fullPath;
      const params = fullPath.substring(1).split('/');
      params.pop();
      const crumbs = [];
      let path = '';

      params.forEach((param, index, { length }) => {
        path = `${path}/${param}`;
        const match = this.$router.match(path);
        if (match.name !== 'index') {
          if (index === length - 1) {
            crumbs.push({
              text: path.replace(/\//g, '_').slice(1),
              disabled: true,
              href: path.replace(/\//g, '_').slice(1),
            });
          } else {
            crumbs.push({
              text: path.replace(/\//g, '_').slice(1),
              disabled: false,
              href: path + '/',
            });
          }
        }
      });
      return crumbs;
    },
  },
  data() {
    return {
      clipped: false,
      drawer: false,
      fixed: false,
      items: [
        {
          icon: 'mdi-apps',
          title: 'Welcome',
          to: '/',
        },
        {
          icon: 'mdi-chart-bubble',
          title: 'Inspire',
          to: '/inspire',
        },
        {
          icon: 'mdi-bell',
          title: 'Example',
          to: '/example',
        },
      ],
      miniVariant: false,
      right: true,
      rightDrawer: false,
      title: 'Vuetify.js',
    };
  },
};
</script>

<style>
body {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
h1 {
  margin: 0;
}
nav {
  width: 150px;
}
ul {
  list-style: none;
  margin: 0;
  padding: 0;
}
li {
  text-align: left;
}
/* nav ul {margin-left: 12px;} */
/* ul ul {padding-top: 6px; border-top: 1px solid #2c3e50;margin-left: 0; margin-top: 6px;} */
.breadcrumb {
  display: flex;
  flex-wrap: wrap;
  padding: 0.75rem 1rem;
  list-style: none;
  background-color: rgb(66, 185, 131, 0.2);
  margin-top: 0;
  margin-bottom: 16px;
}
.breadcrumb-item + .breadcrumb-item {
  padding-left: 6px;
}
.breadcrumb-item + .breadcrumb-item::before {
  display: inline-block;
  color: #2c3e50;
  content: '/';
}
.breadcrumb-item .nuxt-link-active {
  font-weight: bold;
  color: #2c3e50;
  text-decoration: none;
}
.breadcrumb-item .nuxt-link-exact-active.nuxt-link-active {
  color: #42b983;
}
main {
  display: flex;
}
</style>