/*
 * Creado por SharpDevelop.
 * Usuario: Pikachu240
 * Fecha: 05/08/2017
 * Hora: 20:52
 * Licencia GNU GPL V3
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using PokemonGBAFrameWork;
using Gabriel.Cat.Extension;
namespace VisorSpriteEntrenadores
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		List<PokemonGBAFrameWork.ClaseEntrenador> entrenadores;
		
		public Window1()
		{
			entrenadores=new List<ClaseEntrenador>();
			InitializeComponent();
			BtnMenuCargar_Click();
		}
		void BtnMenuCargar_Click(object sender=null, RoutedEventArgs e=null)
		{
			ContextMenu contextMenuImg;
			MenuItem itemMenuContextual;
			RomGba rom;
			EdicionPokemon edicion;
			Compilacion compilacion;
			System.Windows.Controls.Image img;
			
			OpenFileDialog opn=new OpenFileDialog();
			opn.Filter="Pokemon GBA|*.gba";
			if(opn.ShowDialog().GetValueOrDefault())
			{
				rom=new RomGba(opn.FileName);
				edicion=EdicionPokemon.GetEdicionPokemon(rom);
				compilacion=Compilacion.GetCompilacion(rom,edicion);
				
				entrenadores.Clear();
				entrenadores.AddRange(ClaseEntrenador.GetClasesEntrenador(rom,edicion,compilacion));
				
				ugEntrenadores.Children.Clear();
				for(int i=0;i<entrenadores.Count;i++)
				{
					contextMenuImg=new ContextMenu();
					itemMenuContextual=new MenuItem();
					itemMenuContextual.Header="Exportar Sprite";
					itemMenuContextual.Click+=(s,m)=>{
						
					ClaseEntrenador entrenadorAExportar=((ClaseEntrenador)((MenuItem)s).Tag);
					SaveFileDialog sfd=new SaveFileDialog();
					sfd.FileName=entrenadorAExportar.Nombre;
					sfd.DefaultExt="png";
					if(sfd.ShowDialog().GetValueOrDefault())
					{
						((Bitmap)entrenadorAExportar.Sprite).Save(sfd.FileName);
					}else{
						MessageBox.Show("No se ha exportado nada...");
					}
					
					};
					contextMenuImg.Items.Add(itemMenuContextual);
					itemMenuContextual.Tag=entrenadores[i];
					img=new System.Windows.Controls.Image();
					img.SetImage(entrenadores[i].Sprite);
					img.Tag=entrenadores[i];
					img.ContextMenu=contextMenuImg;
					ugEntrenadores.Children.Add(img);
				}
			}
		}
		void BtnMenuExportar_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog fld=new System.Windows.Forms.FolderBrowserDialog();
			int sinNombre=0;
			if(fld.ShowDialog()==System.Windows.Forms.DialogResult.OK)
			{
				for(int i=0;i<entrenadores.Count;i++)
				{
					try{
					((Bitmap)entrenadores[i].Sprite).Save(System.IO.Path.Combine(fld.SelectedPath,entrenadores[i].Nombre+".png"));
					}catch{
						((Bitmap)entrenadores[i].Sprite).Save(System.IO.Path.Combine(fld.SelectedPath,(sinNombre++)+"_sinNombre.png"));
					
					}
				}
				if(sinNombre==0)
				MessageBox.Show("Se ha exportado todo correctamente :)");
				else MessageBox.Show("Se han encontrado algunos nombres no aptos para ser guardados y se ha puesto sinNombre como nombre de archivo, por lo demás todo bien");
			}else{
				MessageBox.Show("No se ha exportado nada");
			}
		}
		void BtnSobre_Click(object sender, RoutedEventArgs e)
		{
			if(MessageBox.Show("Este software esta bajo licencia GNU, ¿Quieres ver el codigo fuente?","Sobre la app",MessageBoxButton.YesNo,MessageBoxImage.Information)==MessageBoxResult.Yes)
				System.Diagnostics.Process.Start("https://github.com/TetradogPokemonGBA/VisorSpriteEntrenadores");
		}
		
	}
}