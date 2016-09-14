﻿using System;
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

        static void Main(string[] args)
        {
            //VARIABLES GLOBALES:
            random = new Random();
            HV = new TimeSpan(23976,0,0);
            T = new TimeSpan(0, 0, 0);
            Tf = new TimeSpan(10800, 0, 0);
            TPLL = T;
            TPSB = HV;
            NSA = 0;
            NSB = 0;

            //CONDICIONES INICIALES
            Console.Write("Ingrese número de empleados Junior:\n");
            N = Convert.ToInt32(Console.ReadLine());

            TPSA = new TimeSpan[N];
            for (int s = 0; s < N; s++)
            {
                TPSA[s] = HV;
            }
            //FIN CI

            while (T < Tf)
            {
                I = menorTPSA();

                //LLEGADA O SALIDA
                if (TPLL <= TPSB)
                {
                    //LLEGADA O SALIDA A
                    if (TPLL <= TPSA[I])
                    {
                        T = TPLL;
                        Ia = calculoIa();
                        TPLL = T + Ia;
                        r = random.NextDouble();

                        if (r < 0.8)
                        {
                            //JUNIOR
                            if (NSA <= N)
                            {
                                NSA++;
                                int j = buscarJunior();
                                salidaA(j);
                            }
                            else {
                                if (NSB==0)
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
                    else {
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
                    else {
                        T = TPSB;
                        NSB--;

                        if (NSB >= 1)
                        {
                            salidaB();
                        }
                        else {
                            if (NSA > N)
                            {
                                NSB++;
                                NSA--;
                                salidaB();
                            }
                            else {
                                TPSB = HV;    
                            }
                        }
                    }
                }
            }
            Console.ReadLine();

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
                salidaB();
            }
        }

        private static void eventoSalidaA()
        {
            T = TPSA[I];
            NSA--;

            if (NSA >= N)
            {
                salidaA(I);
            }
            else {
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
            var x = Convert.ToInt32(rf * (60 + 5) - 5);
            return new TimeSpan(0, x, 0);
        }

        private static TimeSpan calculoTaA()
        {
            var rf = new Random().NextDouble();
            var x = Convert.ToInt32(rf * (60 + 5) - 5);
            return new TimeSpan(0, x, 0);
        }

        private static TimeSpan calculoTaB()
        {
            var rf = new Random().NextDouble();
            var x = Convert.ToInt32(rf * (30 + 10) - 10);
            return new TimeSpan(0, x, 0);
        }

    }
}
