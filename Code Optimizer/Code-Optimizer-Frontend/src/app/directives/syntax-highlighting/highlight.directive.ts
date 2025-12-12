import { Directive, ElementRef, Input, OnChanges } from '@angular/core';
import hljs from 'highlight.js';

@Directive({
  selector: '[highlightCode]'
})
export class HighlightCodeDirective implements OnChanges {

  @Input() code: string = '';
  @Input() language: string = '';

  constructor(private el: ElementRef) {}

  ngOnChanges(): void {
    if (this.code) {
      const highlighted = hljs.highlight(this.code, { language: this.language || 'javascript' }).value;
      this.el.nativeElement.innerHTML = highlighted;
    }
  }
}
