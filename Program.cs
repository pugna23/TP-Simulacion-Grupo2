using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        public static TimeSpan HV { get; set; }
        public static TimeSpan T { get; set; }
        public static TimeSpan Tf { get; set; }
        public static TimeSpan TPLL { get; set; }
        public static TimeSpan TPSB { get; set; }
        public static int N { get; set; }
        public static TimeSpan[] TPSA { get; set; }
        public static int I { get; set; }
        public static Random random{ get; set; }
        public static double r{ get; set; }
        public static TimeSpan Ia{ get; set; }
        public static TimeSpan TaA { get; set; }
        public static TimeSpan TaB { get; set; }
        public static int NSB{ get; set; }
        public static int NSA { get; set; }

        //VAR PARA RESULTADOS
        public static int NT { get; set; }
        public static TimeSpan[] ITOA { get; set; }
        public static TimeSpan ITOB { get; set; }
        public static TimeSpan[] STOA { get; set; }
        public static TimeSpan STOB { get; set; }
        public static double STLL { get; set; }
        public static double STS { get; set; }

        public static long llegadas{ get; set; }
        public static long salidas { get; set; }

        static void Main(string[] args)
        {
        while(true){
            //VARIABLES GLOBALES:

            llegadas = 0;
            salidas = 0;
            Console.Write("\n");
            random = new Random();
            HV = new TimeSpan(239976, 0, 0);
            T = new TimeSpan(0, 0, 0);
            
            TPLL = T;
            TPSB = HV;
            NSA = 0;
            NSB = 0;

            NT = 0;
            ITOB = new TimeSpan(0, 0, 0);
            STOB = new TimeSpan(0, 0, 0);
            STLL = 0;
            STS = 0;

            //CONDICIONES INICIALES
            Console.Write("Ingrese número de empleados Junior:\n");

            N = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ingrese Duración de simulación en HORAS(recomendado: 10800):\n");
            int duracionSim =  Convert.ToInt32(Console.ReadLine());
            Tf = new TimeSpan(duracionSim, 0, 0);

            TPSA = new TimeSpan[N];
            ITOA = new TimeSpan[N];
            STOA = new TimeSpan[N];
            for (int s = 0; s < N; s++)
            {
                TPSA[s] = HV;
                ITOA[s] = new TimeSpan(0, 0, 0);
                STOA[s] = new TimeSpan(0, 0, 0);
            }
            //FIN CI

            while (ejecutarCiclo()) {
            
            };

           
            for (int h = 1; h <= N; h++)
            {
                var PTOA = STOA[h-1].TotalDays / T.TotalDays * 100;
                Console.Write("Porcentaje de Tiempo Ocioso Junior Puesto" + h  + ": " + PTOA + "%\n");
            }
            var PTOB = STOB.TotalDays / T.TotalDays * 100;
            Console.Write("Porcentaje de Tiempo Ocioso (Líder): " + PTOB + "%\n\n");

            Console.Write("Sumatoria de tiempos de llegada: " + STLL+ "\n");
            Console.Write("Sumatoria de tiempos de salida:  " + STS + "\n");
            var PPS = ((STS - STLL) / NT)*1440;
            Console.Write("Promedio de Permanencia en sistema: " + PPS+ " minutos\n");
            Console.ReadLine();
        }
        }

        private static bool ejecutarCiclo()
        {

            do
            {

                I = menorTPSA();

                //LLEGADA O SALIDA
                if (TPLL <= TPSB)
                {
                    //LLEGADA O SALIDA A
                    if (TPLL <= TPSA[I])
                    {
                        STLL = STLL + TPLL.TotalDays;
                        llegadas++;
                        NT++;

                        T = TPLL;
                        Ia = calculoIa();
                        TPLL = T + Ia;
                        r = random.NextDouble();

                        if (r < 0.8)
                        {
                            //JUNIOR
                            NSA++;

                            if (NSA <= N)
                            {
                                int j = buscarJunior();
                                STOA[j] = STOA[j] + (T - ITOA[j]);
                                salidaA(j);
                            }
                            else
                            {
                                if (NSB == 0)
                                {
                                    NSA--;
                                    atiendeLider();
                                }
                            }
                        }
                        else
                        {
                            atiendeLider();
                        }
                    }
                    else
                    {
                        eventoSalidaA();
                    }
                }
                else
                {
                    //SALIDA A O B
                    if (TPSA[I] <= TPSB)
                    {

                        eventoSalidaA();
                    }
                    else
                    {
                        STS = STS + TPSB.TotalDays;
                        salidas++;
                        T = TPSB;
                        NSB--;

                        if (NSB >= 1)
                        {
                            salidaB();
                        }
                        else
                        {
                            if (NSA > N)
                            {
                                NSB++;
                                NSA--;
                                salidaB();
                            }
                            else
                            {
                                ITOB = T;
                                TPSB = HV;
                            }
                        }
                    }
                }
            } while (T < Tf);

            //VACIAMIENTO
            if (NSA == 0)
            {
                if (NSB != 0)
                {
                    TPLL = HV;
                    return true;
                }
            }
            else
            {
                TPLL = HV;
                return true;
            }
            return false;
        }


       
        private static int buscarJunior()
        {
            return Array.IndexOf(TPSA, HV);
        }

        private static void atiendeLider()
        {
            NSB++;
            if (NSB==1)
            {
                STOB = STOB + (T - ITOB);
                salidaB();
            }
        }

        private static void eventoSalidaA()
        {
            STS = STS + TPSA[I].TotalDays;
            salidas++;

            T = TPSA[I];
            NSA--;

            if (NSA >= N)
            {
                salidaA(I);
            }
            else {
                ITOA[I] = T;
                TPSA[I] = HV;
            }
        }

        private static void salidaA(int x)
        {
            
            TaA = calculoTaA();
            TPSA[x] = T + TaA;
        }

        private static void salidaB()
        {
            
            TaB = calculoTaB();
            TPSB= T + TaB;
        }

        private static int menorTPSA()
        {
            return Array.IndexOf(TPSA, TPSA.Min());

        }

        private static TimeSpan calculoIa()
        {
            var rf = new Random().NextDouble();
            var x = Convert.ToInt32(rf * (30 - 10) + 10);
            return new TimeSpan(0, x, 0);
        }

        private static TimeSpan calculoTaA()
        {
            var rf = new Random().NextDouble();
            var x = Convert.ToInt32(rf * (120 - 20) + 20);
            return new TimeSpan(0, x, 0);
        }

        private static TimeSpan calculoTaB()
        {
            var rf = new Random().NextDouble();
            var x = Convert.ToInt32(rf * (60 - 10) + 10);
            return new TimeSpan(0, x, 0);
        }

    }
}
