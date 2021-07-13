import { formatDate } from './formatDate';
import { getFileSizeForDisplay } from './getFileSizeForDisplay';
import { parseFileToBase64 as toBase64 } from './toBase64';

export const getFormattedDate = formatDate;
export const getFileSize= getFileSizeForDisplay;
export const getBase64File = toBase64;
