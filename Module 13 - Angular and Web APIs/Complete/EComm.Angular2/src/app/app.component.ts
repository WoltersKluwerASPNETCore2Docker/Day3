import { Component } 			  from '@angular/core';
import { WebApiService } 		from './web-api.service';
import { Product }          from './product';

@Component({
	selector: 	  'app-root',
	templateUrl: 	'./app.component.html',
	styleUrls: 	  [ './app.component.css' ],
	providers: 	  [ WebApiService ]
})
export class AppComponent {
	products: Product[] = [];
	
	constructor(private webApiService: WebApiService) {}

	public ngOnInit() {
    		this.webApiService
      			.getProducts()
      			.subscribe((products) => { this.products = products; });
  	}
}
