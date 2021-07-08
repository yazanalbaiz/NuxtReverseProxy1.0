<template>
  <v-row>
    <v-col cols="12">
      <ol
        vocab="http://schema.org/"
        typeof="BreadcrumbList"
        class="nuxt-crumbs"
      >
        <li property="itemListElement" typeof="ListItem">
          <NuxtLink property="item" typeof="WebPage" to="/">
            <span property="name"><v-icon>mdi-home</v-icon></span>
          </NuxtLink>
          <v-icon>mdi-chevron-right</v-icon>
          <meta property="position" content="1" />
        </li>
        <li
          v-for="(crumb, index) in crumbs"
          :key="index"
          property="itemListElement"
          typeof="ListItem"
        >
          <NuxtLink
            property="item"
            typeof="WebPage"
            :to="crumb.path"
            v-if="index !== crumbs.length - 1"
          >
            <span property="name">{{ crumb.title }}</span>
          </NuxtLink>
          <strong v-else>{{ crumb.title }}</strong>
          <v-icon v-if="index !== crumbs.length - 1">mdi-chevron-right</v-icon>
          <meta property="position" :content="index + 2" />
        </li>
      </ol>
    </v-col>
  </v-row>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'Breadcrumbs',
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['crumbs']),
  },
  methods: {},
  created() {},
  mounted() {},
};
</script>

<style lang="scss">
.nuxt-crumbs {
  align-items: center;
  display: flex;
  flex-wrap: wrap;
  flex: 0 1 auto;
  list-style-type: none;
  margin: 0;
  padding: 18px 12px;
  a {
    text-decoration: none;
    &:hover {
      color: #273470;
      .v-icon {
        color: #273470;
      }
    }
  }
  li {
    align-items: center;
    display: inline-flex;
    font-size: 14px;
  }
}
</style>