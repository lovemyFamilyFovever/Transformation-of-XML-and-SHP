#!/usr/bin/env python3
# -*- coding: utf-8 -*-
# @Time    : 2020年4月23日 16:24:27
# @Author  : liuxiangchen
# @FileName: xmlToShp.py
# @Software: vscode


import sys
reload(sys)
sys.setdefaultencoding('utf-8')
import arcpy
import os
from arcpy import env

try:
    env.workspace = 'E:/o.cn/镇江市自规局/shp'    
    filepath = env.workspace
    filter=[".txt"]
    result = []
    arcpy.AddMessage('the program has found those files below....')
    for maindir, subdir, file_name_list in os.walk(filepath):
        for filename in file_name_list:

            apath = os.path.join(maindir, filename)
            ext = os.path.splitext(apath)[1] 

            if ext in filter:
                arcpy.AddMessage(apath)
                
                filepathshp=apath.rsplit("/",1)[0]
                filenameshp=apath.rsplit("/",1)[1]
                readfile=open(apath)               
                newfc=arcpy.CreateFeatureclass_management(filepathshp, filenameshp.split('.')[0]+'.shp', 'POINT')   
                Coordinate_System = "PROJCS['ZJ54',GEOGCS['GCS_Beijing_1954',DATUM['D_Beijing_1954',SPHEROID['Krasovsky_1940',6378245.0,298.3]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Gauss_Kruger'],PARAMETER['False_Easting',500000.0],PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',119.5],PARAMETER['Scale_Factor',1.0],PARAMETER['Latitude_Of_Origin',0.0],UNIT['Meter',1.0]]"
                arcpy.DefineProjection_management(newfc,Coordinate_System)                
                arcpy.AddField_management(newfc, "LocationX", "FLOAT",field_precision = 20, field_scale=20)                 
                arcpy.AddField_management(newfc, "LocationY", "FLOAT",field_precision = 20, field_scale=20)                  
                arcpy.AddField_management(newfc, "LocationZ", "FLOAT",field_precision = 20, field_scale=20)                  
                arcpy.AddField_management(newfc, "Matrix3", "TEXT",field_length = 50)
                arcpy.AddField_management(newfc, "ModelName", "TEXT",field_length = 50)              

                inputfile=readfile.readlines()
                cur=arcpy.da.InsertCursor(newfc,["LocationX","LocationY","LocationZ","Matrix3","ModelName","SHAPE@XY"])          
                for row in inputfile:
                    mysplit=row.split('&')   
                    arcpy.AddMessage("Conversion Data:{0}".format(mysplit[0]+'  '+mysplit[1]))             
                    LocationX=mysplit[0]
                    LocationY=mysplit[1]
                    LocationZ=mysplit[2]
                    Matrix3=mysplit[3]
                    ModelName=mysplit[4]
                    position=arcpy.Point(LocationX,LocationY)                                                        
                    newrow=(LocationX,LocationY,LocationZ,Matrix3,ModelName,position)
                    cur.insertRow(newrow)
                del cur
        
except arcpy.ExecuteError:
   arcpy.GetMessages()


