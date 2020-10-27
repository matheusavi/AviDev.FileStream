namespace AviDev.FileStream
{
    public class File
    {
        public File(byte[] arquivo, string nomeArquivo)
        {
            Arquivo = arquivo;
            NomeArquivo = nomeArquivo;
        }

        public int Id { get; set; }
        public byte[] Arquivo { get; set; }
        public string NomeArquivo { get; set; }
    }
}
