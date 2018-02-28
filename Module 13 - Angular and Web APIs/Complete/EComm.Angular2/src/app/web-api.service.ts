import { Injectable }       from '@angular/core';
import { environment }      from 'environments/environment';
import { Http, Response }   from '@angular/http';
import { Product }          from './product';

import { Observable }       from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

const API_URL = environment.apiUrl;

@Injectable()
export class WebApiService {
    
	constructor(private http: Http) { }
  
  // API: GET /Product
  public getProducts() {
	console.log(API_URL);
    	return this.http
		    .get(API_URL + 'Product')
    		.map(response => {
      			const products = response.json();
      			return products.map((product) => new Product(product));
    		})
    		.catch(this.handleError);
	}

	private handleError (error: Response | any) {
  		console.error('ApiService::handleError', error);
  		return Observable.throw(error);
	}
}
