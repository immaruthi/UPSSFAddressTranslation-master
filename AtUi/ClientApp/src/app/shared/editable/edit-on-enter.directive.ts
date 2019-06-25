import { Directive, Input, HostListener } from '@angular/core';
import { EditableComponent } from './editable.component';

@Directive({
  selector: '[editOnEnter]'
})
export class EditOnEnterDirective {

  constructor(private editable: EditableComponent) {
  }

  @HostListener('keyup.enter')
  onEnter() {
    //this.editable.toViewMode();
  }

}
