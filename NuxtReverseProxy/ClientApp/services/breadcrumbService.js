export default class BreadcrumbService {
  constructor(store) {
    this.store = store;
  }

  getCrumbs() {
    return this.store.getters.crumbs;
  }
  setCrumbs(crumbs) {
    this.store.dispatch('setCrumbs', crumbs);
  }
}
