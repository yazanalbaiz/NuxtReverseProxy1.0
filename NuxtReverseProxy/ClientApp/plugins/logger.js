export default function (_, inject) {
  const labelStyle =
    'background:#35495e ; padding: 5px; border-radius: 5px;  color: #fff';
  const loggerService = (...args) => {
    const label = `%c${args[0]}`;
    const values = ['\n\n'].concat(args.filter((_, i) => i > 0).reduce((r, a) => r.concat(a, '\n\n'), []));
    console.log(label, labelStyle, ...values);
  };

  // Will be available in the components as this.$myService
  inject('log', loggerService);
}
