import { Injectable } from '@angular/core';
import prettier from "prettier/standalone";
import parserTypescript from "prettier/plugins/typescript";
import parserHtml from "prettier/plugins/html";
import parserPostCss from "prettier/plugins/postcss";
import parserEstree from "prettier/plugins/estree";
import parserBabel from "prettier/plugins/babel";
import { format as sqlFormat } from "sql-formatter";

@Injectable({
  providedIn: 'root'
})
export class CodeFormatterService {

  async format(code: string, language: string): Promise<string> {
    language = language.toLowerCase().trim();

    try {

      if (language === "sql") {
        return sqlFormat(code, {
          language: "sql",
        });
      }

      if (["c#", "cs", "csharp"].includes(language)) {
        return code;
      }

      const parser = this.getParser(language);

      return prettier.format(code, {
        parser,
        plugins: [
          parserTypescript,
          parserHtml,
          parserPostCss,
          parserEstree
        ],
      });

    } catch (err) {
      console.error("Formatting error:", err);
      return code; 
    }
  }

  private getParser(language: string): string {
    switch (language) {
      case "ts":
      case "typescript":
        return "typescript";

      case "js":
      case "javascript":

        return "typescript";

      case "html":
        return "html";

      case "css":
        return "css";

      default:
        return "typescript";
    }
  }
}
