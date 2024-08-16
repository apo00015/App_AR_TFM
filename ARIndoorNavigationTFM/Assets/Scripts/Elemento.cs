using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Elemento
{
    public string id_edificio { get; set; }
    public string id_elemento { get; set; }
    public string nombre_edificio { get; set; }
    public string nombre_elemento { get; set; }
    public List<string> posiciones { get; set; }
    public List<string> descripciones { get; set; }
}