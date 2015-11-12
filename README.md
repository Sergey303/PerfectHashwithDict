# PerfectHashwithDict

  
   
            var n = (int)(strings.Length * 100); //максимальный код
            
            var coded = new string[n];
            
            var haveCodes = new bool[n];
            
            Queue<string> withCollision=new Queue<string>();
            
            for (int i = 0; i < strings.Length; i++)
            {
                var hash = strings[i].GetHash(); 
                
                //string -> utf32 -> bytes -> modified Brensteinh hash -> bytes -> first 32 bits -> int
                
                var code = hash.GetCode(n); // hash % n
                
                if (haveCodes[code])//collision
                {
                    withCollision.Enqueue(strings[i]);
                }
                else // first code occurence
                {

                    haveCodes[code] = true;
                    coded[code] = strings[i];
                }
            }
            
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < n; i++)
            {
                if(haveCodes[i]) continue;

                // i - свободный код
                
                var s = withCollision.Dequeue();
                dict.Add(s, i);

                coded[i] = s;

                // все коллизии размещены
                if(!withCollision.Any()) break;
            }
