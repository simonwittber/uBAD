using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BAD
{
    public class Tokenizer
    {


        public static string[] Tokenize (string code)
        {
            var tokens = new List<string>();
            var chars = new Queue<char>(code);
            var token = "";
            var collectString = false;
            while(chars.Count > 0) {
                var c = chars.Dequeue();
                if(collectString) {
                    if(c == '"' && !token.EndsWith("\"")) {
                        collectString = false;
                        token += c;
                        tokens.Add(token);
                        token = "";
                        continue;
                    } 
                } else {
                    if(" \t\n\r{},".IndexOf(c) != -1) {
                        if(token.Length > 0) {
                            tokens.Add(token);
                            token = "";
                        }
                        if(c == '\n') 
                            tokens.Add("\n");
                        if(c == '{') {
                            tokens.Add("\n");
                            tokens.Add("{");
                        }
                        if(c == '}') {
                            tokens.Add("\n");
                            tokens.Add("}");
                        }
                        continue;
                    }
                    if(c == '"') {
                        collectString = true;
                    }
                }
                token += c;
            }
            if(token.Length > 0)
                tokens.Add(token);
            return tokens.ToArray();
        }



    
    }
}