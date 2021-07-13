export default function ({ app }) {
  app.router.beforeEach((to, from, next) => {
    if (to.name.includes('test')) {
      console.log('hello :)');
    }

    if (from.name.includes('test')) {
      console.log('bye :(');
    }

    next();
  });
}
