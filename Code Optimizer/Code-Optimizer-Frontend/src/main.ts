import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

(window as any).MonacoEnvironment = {
  getWorkerUrl(_: string, label: string) {
    const base = 'assets/monaco/vs';

    if (label === 'json') {
      return new URL(`${base}/base/worker/workerMain.js?worker=json`, import.meta.url).toString();
    }
    if (label === 'css' || label === 'scss' || label === 'less') {
      return new URL(`${base}/base/worker/workerMain.js?worker=css`, import.meta.url).toString();
    }
    if (label === 'html' || label === 'handlebars' || label === 'razor') {
      return new URL(`${base}/base/worker/workerMain.js?worker=html`, import.meta.url).toString();
    }
    if (label === 'typescript' || label === 'javascript') {
      return new URL(`${base}/base/worker/workerMain.js?worker=ts`, import.meta.url).toString();
    }

    // generic editor worker
    return new URL(`${base}/base/worker/workerMain.js?worker=editor`, import.meta.url).toString();
  }
};
bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
