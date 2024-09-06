import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter',
  standalone: true
})
export class FilterPipe implements PipeTransform {

  transform(items: any[], searchText: string): any[] {
    if (!items || !searchText) return items;

    console.log('Items:', items); 
    console.log('Search Text:', searchText); 

    searchText = searchText.toLowerCase();

    return items.filter(item => {
      const searchField = item.formName || item.title || item.description;
      if (searchField) {
        return searchField.toLowerCase().includes(searchText);
      }
      return false;
    });
  }

}
