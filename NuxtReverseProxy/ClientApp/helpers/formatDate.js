import moment from 'moment';
export function formatDate(date, format = 'DD/MM/YYYY') {
  return moment(date).format(format);
}
