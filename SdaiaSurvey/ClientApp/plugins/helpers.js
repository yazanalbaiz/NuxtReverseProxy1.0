import { getFormattedDate, getFileSize, getBase64File } from '@/helpers';
export default function (_, inject) {
  const helpers = {
    getFormattedDate,
    getFileSize,
    getBase64File,
  };

  // Will be available in the components as this.$helpers
  inject('helpers', helpers);
}
