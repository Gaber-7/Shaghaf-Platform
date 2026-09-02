import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss']
})
export class TopbarComponent {
  constructor(public translate: TranslateService) {
    // set default language
    this.translate.addLangs(['en','ar']);
    this.translate.setDefaultLang('en');
    this.translate.use('en');
  }

  switchLang(lang: string){
    this.translate.use(lang);
    document.documentElement.lang = lang;
    document.documentElement.dir = (lang === 'ar') ? 'rtl' : 'ltr';
  }
}
