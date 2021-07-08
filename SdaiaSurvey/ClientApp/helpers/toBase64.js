export async function parseFileToBase64(file) {
  let fileModel = {};
  fileModel.name = file.name;
  fileModel.contentType = file.type;

  const toBase64 = (file) =>
    new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = (error) => reject(error);
    });

  fileModel.base64Data = await toBase64(file);

  return fileModel;
}
