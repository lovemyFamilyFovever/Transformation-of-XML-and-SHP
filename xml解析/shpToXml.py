#!/usr/bin/env python3
# -*- coding: utf-8 -*-
# @Time    : 2020年4月23日 23:24:27
# @Author  : liuxiangchen
# @FileName: shpToXml.py
# @Software: vscode
import codecs
import sys
reload(sys)
sys.setdefaultencoding('utf-8')
import arcpy
import os
from arcpy import env
from xml.dom.minidom import Document

try:
    env.workspace = 'E:\shp'
    filepath = env.workspace
    filter=[".shp"]
    result = []
    for maindir, subdir, file_name_list in os.walk(filepath):
        for filename in file_name_list:

            apath = os.path.join(maindir, filename)
            ext = os.path.splitext(apath)[1] 

            if ext in filter:
                
                newXmlPath = apath.replace(".shp",".xml")
                arcpy.AddMessage(apath)
                #C:/Users/Administrator/Desktop/temp/主核心区现状地面.shp

                with arcpy.da.SearchCursor(apath,["ModelName","SHAPE@X","SHAPE@Y","LocationZ","Matrix3"]) as cursor:
                    
                    doc = Document()
                    ModelPointClass=doc.createElement("ModelPointClass")
                    doc.appendChild(ModelPointClass)
                    for row in cursor:

                        objectE = doc.createElement("ModelPoint")


                        objectModelName = doc.createElement("ModelName")
                        objectModelNametext = doc.createTextNode(str(row[0]).strip())
                        objectModelName.appendChild(objectModelNametext)
                        objectE.appendChild(objectModelName)

                        
                        objectPOINT_X = doc.createElement("LocationX")
                        objectPOINT_Xtext = doc.createTextNode(str(row[1]))
                        objectPOINT_X.appendChild(objectPOINT_Xtext)
                        objectE.appendChild(objectPOINT_X)

                        objectPOINT_Y = doc.createElement("LocationY")
                        objectPOINT_Ytext = doc.createTextNode(str(row[2]))
                        objectPOINT_Y.appendChild(objectPOINT_Ytext)
                        objectE.appendChild(objectPOINT_Y)

                        objectLocationZ = doc.createElement("LocationZ")
                        objectLocationZtext = doc.createTextNode(str(row[3]))
                        objectLocationZ.appendChild(objectLocationZtext)
                        objectE.appendChild(objectLocationZ)

                        objectMatrix3 = doc.createElement("Matrix3")
                        objectMatrix3text = doc.createTextNode(str(row[4]))
                        objectMatrix3.appendChild(objectMatrix3text)
                        objectE.appendChild(objectMatrix3)


                        ModelPointClass.appendChild(objectE)

                    f = codecs.open(newXmlPath, 'w','gbk')                    
                    doc.writexml(f, indent='\t', newl='\n', addindent='\t', encoding='gb2312')
                    f.close()
                    arcpy.AddMessage(newXmlPath+' finish')
    arcpy.AddMessage('all the files complete!!!')

except arcpy.ExecuteError:
   arcpy.GetMessages()









