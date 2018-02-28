export class Product {
  id: number;
  productName = '';
  unitPrice = 0;

  constructor(values: Object = {}) {
    Object.assign(this, values);
  }
}
