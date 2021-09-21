﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamemisOffLine.Asset
{
    public class TitlePrint
    {

        public string[][] titel = new string[][]
        {
            new string[]{"សរុបតាមឆ្នាំសិក្សា","តាមកម្រិតរួម", "តាមកម្រិតនីមួយៗ", "តាមថ្នាក់នីមួយៗ", "ប្រឡងឡើងថ្នាក់","ត្រួតថ្នាក់","បោះបង់ការសិក្សា"},
            new string[]{"កាលវិភាគសិក្សាតាមថ្នាក់"},
            new string[]{"លទ្ធផលសិក្សាតាមថ្នាក់","ព្រិត្តប័ត្តពិន្ទុសិស្ស","តារាងកិត្តិយសតាមថ្នាក់","ចំណាត់ថ្នាក់ ចំណាត់ប្រភេទប្រចាំឆមាសតាមថ្នាក់","លទ្ធផលសិក្សាគ្រប់មុខវិជ្ជា","តារាងសង្ខេបនិន្ទេសសិស្សតាមមុខវិជ្ជាគោល" },
            new string[]{"កាតសិស្សតាមថ្នាក់"},
            new string[]{"បញ្ចីអវត្តមានសិស្សតាមថ្នាក់"},
            new string[]{"តាមថ្នាក់នីមួយៗ","សរុបតាមកម្រិតនីមួយៗ"},
            new string[]{"ចំណាត់ថ្នាក់ ចំណាត់ប្រភេទប្រចាំឆមាសតាមថ្នាក់"},
            new string[]{"តាមថ្នាក់នីមួយៗ", "សរុបតាមកម្រិតនីមួយៗ" },
            new string[]{"សញ្ញាប័ត្ររបស់បុគ្គលិក","និទ្ទេសតាមថ្នាក់","និទ្ទេស​តាមកម្រិត​"},
            new string[]{ "ចំណេះទូទៅថ្នាក់ទី៩ និងបំពេញវិជ្ជាថ្នាក់ទី៩", "ចំណេះទូទៅថ្នាក់ទី១២​ និងបំពេញវិជ្ជាថ្នាក់ទី១២"},
            new string[]{"សិស្សសរុបតាមថ្នាក់នីមួយៗ"},
            new string[]{"និទ្ទេសមុខវិជ្ជាតាមថ្នាក់", "និទ្ទេសមុខវិជ្ជាតាមកម្រិត" },
            new string[]{"តាមថ្នាក់នីមួយៗ", "សរុបតាមកម្រិតនីមួយៗ" },
        };
        public string[][] titleStaff = new string[][] {
        new string[]{"ប្រវត្តិរូប​បុគ្គលិក និងគ្រូម្នាក់ៗ"},
        new string[]{"លិខិតបញ្ចាក់ការងារបុគ្គលិក និងគ្រូម្នាក់ៗ"},
        new string[]{"បំណែងចែកភារកិច្ចបុគ្គលិកសិក្សា","បញ្ចីឈ្មោះបុគ្គលិក និងគ្រូតាមឆ្នាំសិក្សា", "បំណែងចែកគ្រូទទួលបន្ទុកថ្នាក់", "ឈ្មោះមុខវិជ្ជាបង្រៀន និងចំនួនម៉ោងបង្រៀន"},
        new string[]{"បំណែងចែកតួនាទីនិងភារកិច្ចគ្រូ","បញ្ចីឈ្មោះបុគ្គលិកនិងគ្រូតាមឆ្នាំសិក្សា","គ្រូទទួលបន្ទុកថ្នាក់","ឈ្មោះមុខវិជ្ចាបង្រៀន និងម៉ោងបង្រៀន"},
        new string[]{"សិត្ថិសិស្ស តាមកម្រិតថ្នាក់", "សិត្ថិគ្រូបង្រៀនតាមប្រភេទក្រុខណ្ឌ", "សិត្ថិបុគ្គលិកទីចាត់ការតាមប្រភេទក្រខណ្ឌ"},
        new string[]{"កាលវិភាគរួម","កាលវិភាគគ្រូ"},
        new string[]{"បញ្ចីអវត្តមានគ្រូនិងបុគ្គលិកប្រចាំខែ","បញ្ចីអវត្តមានគ្រួនិងបុគ្គលិកប្រចាំឆមាស"},
        };

        public string[] discriptionStudent = {
            "ការនែនាំ \n ១-ជ្រើសរើសឆ្នាំសិក្សា \n ២-ជ្រើសរើសកម្រិត \n ៣-ជ្រើសរើសថ្នាក់",
            "មុខងារកំពុងសាងសង់",
            "ការនែនាំ \n ១-ជ្រើសរើសឆ្នាំសិក្សា \n ២-ជ្រើសរើសកម្រិត \n ៣-ជ្រើសរើសថ្នាក់ \n ៤-ជ្រើសរើសខែ/ឆមាស",
            "ការនែនាំ \n ១-ជ្រើសរើសឆ្នាំសិក្សា \n ២-ជ្រើសរើសកម្រិត \n ៣-ជ្រើសរើសថ្នាក់",
            "មុខងារកំពុងសាងសង់",
            "មុខងារកំពុងសាងសង់",
            "ការនែនាំ \n ១-ជ្រើសរើសឆ្នាំសិក្សា \n ២-ជ្រើសរើសកម្រិត \n ៣-ជ្រើសរើសថ្នាក់ \n ៤-ជ្រើសរើសប្រភេទនិទ្ទេស",
            "ការនែនាំ \n ១-ជ្រើសរើសឆ្នាំសិក្សា \n ២-ជ្រើសរើសកម្រិត \n ៣-ជ្រើសរើសថ្នាក់ \n ៤-ជ្រើសរើសមុខវិជ្ជា \n ៥-ជ្រើសរើសភេទ",
            "មុខងារកំពុងសាងសង់",
            "ការនែនាំ \n ១-ជ្រើសរើសឆ្នាំសិក្សា \n ២-ជ្រើសរើសកម្រិត \n ៣-ជ្រើសរើសថ្នាក់",
            "មុខងារកំពុងសាងសង់",
            "មុខងារកំពុងសាងសង់",
            "មុខងារកំពុងសាងសង់",
        };
        public string[] discriptionStaff =
        {
            "មុខងារកំពុងសាងសង់",
        };
        
    }
}